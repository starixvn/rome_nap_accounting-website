<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:param name="IsInternal" select="1"></xsl:param>
  <xsl:output method="html"/>
  <xsl:template match="acc_custoverrideupdhr">
    <xsl:choose>
      <xsl:when test="@custid">
        <table class="tblDesc">
          <tr>
            <td class="colW1">
              {lbl:checkPermission}
            </td>
            <td class="colW2">
              <xsl:value-of select="@checkpermission" />
            </td>
            <td class="colW1">
              {lbl:updMinPos}
            </td>
            <td class="colW2">
              <xsl:value-of select="@updminpos" />
            </td>
            <td class="colW1">
              {lbl:updMaxPos}
            </td>
            <td class="colW2">
              <xsl:value-of select="@updmaxpos" />
            </td>
          </tr>
          <tr>
            <td class="colW1">
              {lbl:language}
            </td>
            <td class="colW2">
              <xsl:value-of select="@language" />
            </td>
            <td class="colW1">
              {lbl:subAccName}
            </td>
            <td class="colW2">
              <xsl:value-of select="@subaccname" />
            </td>
            <td class="colW1">
              {lbl:writeLog}
            </td>
            <td class="colW2">
              <xsl:value-of select="@writelog" />
            </td>
          </tr>
          <tr>
            <td class="colW1">
              {lbl:ip}
            </td>
            <td class="colW2">
              <xsl:value-of select="@ip" />
            </td>

            <td class="colW1">
              {lbl:errCode}
            </td>
            <td class="colW2">
              <xsl:value-of select="@errcode" />
            </td>
            <td class="colW1">
              {lbl:errMsg}
            </td>
            <td class="colW2">
              <xsl:value-of select="@errmsg" />
            </td>
          </tr>
          <xsl:choose>
            <xsl:when test="@minv151pt_31">
              <tr class="headerDetailNormal">
                <td colspan="6">{lbl:minMaxPT}</td>
              </tr>
              <tr >
                <td class="colW1">
                  {lbl:minV151PT_31}
                </td>
                <td class="colW2">
                  <xsl:value-of select="@minv151pt_31" />
                </td>
                <td class="colW1">
                  {lbl:maxV151PT_31}
                </td>
                <td class="colW2">
                  <xsl:value-of select="@maxv151pt_31" />
                </td>
                <td class="colW1">
                  {lbl:minV151PT_32}
                </td>
                <td class="colW2">
                  <xsl:value-of select="@minv151pt_32" />
                </td>
              </tr>
              <tr >
                <td class="colW1">
                  {lbl:maxV151PT_32}
                </td>
                <td class="colW2">
                  <xsl:value-of select="@maxv151pt_32" />
                </td>
                <td class="colW1">
                  {lbl:minv151pt_41}
                </td>
                <td class="colW2">
                  <xsl:value-of select="@minv151pt_41" />
                </td>
                <td class="colW1">
                  {lbl:maxv151pt_41}
                </td>
                <td class="colW2">
                  <xsl:value-of select="@maxv151pt_41" />
                </td>
              </tr>
              <tr >
                <td class="colW1">
                  {lbl:minv151pt_42}
                </td>
                <td class="colW2">
                  <xsl:value-of select="@minv151pt_42" />
                </td>
                <td class="colW1">
                  {lbl:maxv151pt_42}
                </td>
                <td class="colW2">
                  <xsl:value-of select="@maxv151pt_42" />
                </td>
                <td class="colW1">

                </td>
                <td class="colW2">

                </td>
              </tr>
            </xsl:when>
          </xsl:choose>
          <xsl:for-each select="minmaxpt">
            <tr class="headerDetailNormal">
              <td colspan="6">{lbl:minMaxPT}</td>
            </tr>
            <tr >
              <td class="colW1">
                {lbl:minV151PT_31}
              </td>
              <td class="colW2">
                <xsl:value-of select="@minv151pt_31" />
              </td>
              <td class="colW1">
                {lbl:maxV151PT_31}
              </td>
              <td class="colW2">
                <xsl:value-of select="@maxv151pt_31" />
              </td>

              <td class="colW1">
                {lbl:minV151PT_32}
              </td>
              <td class="colW2">
                <xsl:value-of select="@minv151pt_32" />
              </td>
            </tr>
            <tr >
              <td class="colW1">
                {lbl:maxV151PT_32}
              </td>
              <td class="colW2">
                <xsl:value-of select="@maxv151pt_32" />
              </td>
              <td class="colW1">
                {lbl:minv151pt_41}
              </td>
              <td class="colW2">
                <xsl:value-of select="@minv151pt_41" />
              </td>
              <td class="colW1">
                {lbl:maxV151PT_41}
              </td>
              <td class="colW2">
                <xsl:value-of select="@maxv151pt_41" />
              </td>
            </tr>
            <tr >
              <td class="colW1">
                {lbl:minv151pt_42}
              </td>
              <td class="colW2">
                <xsl:value-of select="@minv151pt_42" />
              </td>
              <td class="colW1">
                {lbl:maxv151pt_42}
              </td>
              <td class="colW2">
                <xsl:value-of select="@maxv151pt_42" />
              </td>
              <td class="colW1">
                {lbl:minv151pt_51}
              </td>
              <td class="colW2">
                <xsl:value-of select="@minv151pt_51" />
              </td>
            </tr>
            <tr >
              <td class="colW1">
                {lbl:maxv151pt_51}
              </td>
              <td class="colW2">
                <xsl:value-of select="@maxv151pt_51" />
              </td>
              <td class="colW1">
                {lbl:minv151pt_52}
              </td>
              <td class="colW2">
                <xsl:value-of select="@minv151pt_52" />
              </td>
              <td class="colW1">
                {lbl:maxv151pt_52}
              </td>
              <td class="colW2">
                <xsl:value-of select="@maxv151pt_52" />
              </td>
            </tr>
            <tr >
              <td class="colW1">
                {lbl:minv152pt_41}
              </td>
              <td class="colW2">
                <xsl:value-of select="@minv152pt_41" />
              </td>
              <td class="colW1">
                {lbl:maxv152pt_41}
              </td>
              <td class="colW2">
                <xsl:value-of select="@maxv152pt_41" />
              </td>
              <td class="colW1">
                {lbl:minv152pt_42}
              </td>
              <td class="colW2">
                <xsl:value-of select="@minv152pt_42" />
              </td>
            </tr>
            <tr >
              <td class="colW1">
                {lbl:maxv152pt_42}
              </td>
              <td class="colW2">
                <xsl:value-of select="@maxv152pt_42" />
              </td>
              <td class="colW1">
                {lbl:minv152pt_51}
              </td>
              <td class="colW2">
                <xsl:value-of select="@minv152pt_51" />
              </td>
              <td class="colW1">
                {lbl:maxv152pt_51}
              </td>
              <td class="colW2">
                <xsl:value-of select="@maxv152pt_51" />
              </td>
            </tr>
            <tr >
              <td class="colW1">
                {lbl:minv152pt_52}
              </td>
              <td class="colW2">
                <xsl:value-of select="@minv152pt_52" />
              </td>
              <td class="colW1">
                {lbl:maxv152pt_52}
              </td>
              <td class="colW2">
                <xsl:value-of select="@maxv152pt_52" />
              </td>
              <td class="colW1">
                {lbl:minv153pt_51}
              </td>
              <td class="colW2">
                <xsl:value-of select="@minv153pt_51" />
              </td>
            </tr>
            <tr >
              <td class="colW1">
                {lbl:maxv153pt_51}
              </td>
              <td class="colW2">
                <xsl:value-of select="@maxv153pt_51" />
              </td>
              <td class="colW1">
                {lbl:minv153pt_52}
              </td>
              <td class="colW2">
                <xsl:value-of select="@minv153pt_52" />
              </td>
              <td class="colW1">
                {lbl:maxv153pt_52}
              </td>
              <td class="colW2">
                <xsl:value-of select="@maxv153pt_52" />
              </td>
            </tr>
          </xsl:for-each>
        </table>
      </xsl:when>
      <xsl:otherwise>
        <table class="tblDesc">
          <xsl:for-each select="info">
            <tr class="headerDetailNormal">
              <td colspan="6">{lbl:custInfo}</td>
            </tr>
            <tr >

              <td class="colW1">
                {lbl:checkPermission}
              </td>
              <td class="colW2">
                <xsl:value-of select="@checkpermission" />
              </td>
              <td class="colW1">
                {lbl:updMinPos}
              </td>
              <td class="colW2">
                <xsl:value-of select="@updminpos" />
              </td>

              <td class="colW1">
                {lbl:updMaxPos}
              </td>
              <td class="colW2">
                <xsl:value-of select="@updmaxpos" />
              </td>
            </tr>

            <tr >
              <td class="colW1">
                {lbl:language}
              </td>
              <td class="colW2">
                <xsl:value-of select="@language" />
              </td>
              <td class="colW1">
                {lbl:subAccName}
              </td>
              <td class="colW2">
                <xsl:value-of select="@subaccname" />
              </td>
              <td class="colW1">
                {lbl:writeLog}
              </td>
              <td class="colW2">
                <xsl:value-of select="@writelog" />
              </td>
            </tr>

            <tr >
              <td class="colW1">
                {lbl:ip}
              </td>
              <td class="colW2">
                <xsl:value-of select="@ip" />
              </td>

              <td class="colW1">
                {lbl:errCode}
              </td>
              <td class="colW2">
                <xsl:value-of select="@errcode" />
              </td>
              <td class="colW1">
                {lbl:errMsg}
              </td>
              <td class="colW2">
                <xsl:value-of select="@errmsg" />
              </td>

            </tr>
          </xsl:for-each>
          <xsl:for-each select="minmaxpt">
            <tr class="headerDetailNormal">
              <td colspan="6">{lbl:minMaxPT}</td>
            </tr>

            <tr >
              <td class="colW1">
                {lbl:minV151PT_31}
              </td>
              <td class="colW2">
                <xsl:value-of select="@minv151pt_31" />
              </td>
              <td class="colW1">
                {lbl:maxV151PT_31}
              </td>
              <td class="colW2">
                <xsl:value-of select="@maxv151pt_31" />
              </td>

              <td class="colW1">
                {lbl:minV151PT_32}
              </td>
              <td class="colW2">
                <xsl:value-of select="@minv151pt_32" />
              </td>
            </tr>
            <tr >
              <td class="colW1">
                {lbl:maxV151PT_32}
              </td>
              <td class="colW2">
                <xsl:value-of select="@maxv151pt_32" />
              </td>
              <td class="colW1">
                {lbl:minv151pt_41}
              </td>
              <td class="colW2">
                <xsl:value-of select="@minv151pt_41" />
              </td>
              <td class="colW1">
                {lbl:maxV151PT_41}
              </td>
              <td class="colW2">
                <xsl:value-of select="@maxv151pt_41" />
              </td>
            </tr>
            <tr >
              <td class="colW1">
                {lbl:minv151pt_42}
              </td>
              <td class="colW2">
                <xsl:value-of select="@minv151pt_42" />
              </td>
              <td class="colW1">
                {lbl:maxv151pt_42}
              </td>
              <td class="colW2">
                <xsl:value-of select="@maxv151pt_42" />
              </td>
              <td class="colW1">
                {lbl:minv151pt_51}
              </td>
              <td class="colW2">
                <xsl:value-of select="@minv151pt_51" />
              </td>
            </tr>
            <tr >
              <td class="colW1">
                {lbl:maxv151pt_51}
              </td>
              <td class="colW2">
                <xsl:value-of select="@maxv151pt_51" />
              </td>
              <td class="colW1">
                {lbl:minv151pt_52}
              </td>
              <td class="colW2">
                <xsl:value-of select="@minv151pt_52" />
              </td>
              <td class="colW1">
                {lbl:maxv151pt_52}
              </td>
              <td class="colW2">
                <xsl:value-of select="@maxv151pt_52" />
              </td>
            </tr>
            <tr >
              <td class="colW1">
                {lbl:minv152pt_41}
              </td>
              <td class="colW2">
                <xsl:value-of select="@minv152pt_41" />
              </td>
              <td class="colW1">
                {lbl:maxv152pt_41}
              </td>
              <td class="colW2">
                <xsl:value-of select="@maxv152pt_41" />
              </td>
              <td class="colW1">
                {lbl:minv152pt_42}
              </td>
              <td class="colW2">
                <xsl:value-of select="@minv152pt_42" />
              </td>
            </tr>
            <tr >
              <td class="colW1">
                {lbl:maxv152pt_42}
              </td>
              <td class="colW2">
                <xsl:value-of select="@maxv152pt_42" />
              </td>
              <td class="colW1">
                {lbl:minv152pt_51}
              </td>
              <td class="colW2">
                <xsl:value-of select="@minv152pt_51" />
              </td>
              <td class="colW1">
                {lbl:maxv152pt_51}
              </td>
              <td class="colW2">
                <xsl:value-of select="@maxv152pt_51" />
              </td>
            </tr>
            <tr >
              <td class="colW1">
                {lbl:minv152pt_52}
              </td>
              <td class="colW2">
                <xsl:value-of select="@minv152pt_52" />
              </td>
              <td class="colW1">
                {lbl:maxv152pt_52}
              </td>
              <td class="colW2">
                <xsl:value-of select="@maxv152pt_52" />
              </td>
              <td class="colW1">
                {lbl:minv153pt_51}
              </td>
              <td class="colW2">
                <xsl:value-of select="@minv153pt_51" />
              </td>
            </tr>
            <tr >
              <td class="colW1">
                {lbl:maxv153pt_51}
              </td>
              <td class="colW2">
                <xsl:value-of select="@maxv153pt_51" />
              </td>
              <td class="colW1">
                {lbl:minv153pt_52}
              </td>
              <td class="colW2">
                <xsl:value-of select="@minv153pt_52" />
              </td>
              <td class="colW1">
                {lbl:maxv153pt_52}
              </td>
              <td class="colW2">
                <xsl:value-of select="@maxv153pt_52" />
              </td>
            </tr>
          </xsl:for-each>
        </table>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
</xsl:stylesheet>
