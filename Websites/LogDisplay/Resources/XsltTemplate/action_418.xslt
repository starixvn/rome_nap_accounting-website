<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:param name="IsInternal" select="1"></xsl:param>
  <xsl:output method="html"/>
  <xsl:template match="acc_customerupdsuperhr_positiontaking">
    <table class="tblDesc">
      <xsl:for-each select="info">
        <tr class="headerDetailNormal" >
          <td colspan="6">{lbl:custInfo}</td>
        </tr>
        <tr >

          <td class="colW1">{lbl:IP}</td>
          <td class="colW2">
            <xsl:value-of select="@ip" />
          </td>
          <td class="colW1">{lbl:ErrCode}</td>
          <td class="colW2">
            <xsl:value-of select="@errcode" />
          </td>
          <td class="colW1">{lbl:Language}</td>
          <td class="colW2">
            <xsl:value-of select="@language" />
          </td>
        </tr>
        <tr >

          <td class="colW1">{lbl:ErrMsg}</td>
          <td class="colW2"  >
            <xsl:value-of select="@errmsg" />
          </td>
          <td class="colTitle"></td>
          <td class="colValue">

          </td>
          <td class="colTitle"></td>
          <td class="colValue">

          </td>
        </tr>
      </xsl:for-each>
      <xsl:for-each select="s">
        <tr class="headerDetailNormal" >
          <td colspan="6">{lbl:s}</td>
        </tr>
        <tr >
          <td class="colW1">{lbl:v151spt_31}</td>
          <td class="colW2">
            <xsl:value-of select="@v151spt_31" />
          </td>
          <td class="colW1">{lbl:v151spt_32}</td>
          <td class="colW2">
            <xsl:value-of select="@v151spt_32" />
          </td>
          <td class="colW1">{lbl:v151spt_41}</td>
          <td class="colW2">
            <xsl:value-of select="@v151spt_41" />
          </td>
        </tr>
        <tr>
          <td class="colW1">{lbl:v151spt_42}</td>
          <td class="colW2">
            <xsl:value-of select="@v151spt_42" />
          </td>
          <td class="colW1">{lbl:v151spt_51}</td>
          <td class="colW2">
            <xsl:value-of select="@v151spt_51" />
          </td>
          <td class="colW1">{lbl:v151spt_52}</td>
          <td class="colW2">
            <xsl:value-of select="@v151spt_52" />
          </td>
        </tr>
        <tr>
          <td class="colW1">{lbl:v152spt_41}</td>
          <td class="colW2">
            <xsl:value-of select="@v152spt_41" />
          </td>
          <td class="colW1">{lbl:v152spt_42}</td>
          <td class="colW2">
            <xsl:value-of select="@v152spt_42" />
          </td>
          <td class="colW1">{lbl:v152spt_51}</td>
          <td class="colW2">
            <xsl:value-of select="@v152spt_51" />
          </td>
        </tr>
        <tr>
          <td class="colW1">{lbl:v152spt_52}</td>
          <td class="colW2">
            <xsl:value-of select="@v152spt_52" />
          </td>
          <td class="colW1">{lbl:v153spt_51}</td>
          <td class="colW2">
            <xsl:value-of select="@v153spt_51" />
          </td>
          <td class="colW1">{lbl:v153spt_52}</td>
          <td class="colW2">
            <xsl:value-of select="@v153spt_52" />
          </td>
        </tr>
      </xsl:for-each>
      
      <xsl:for-each select="m">
        <tr class="headerDetailNormal" >
          <td colspan="6">{lbl:m}</td>
        </tr>
        <tr >
          <td class="colW1">{lbl:v151mpt_31}</td>
          <td class="colW2">
            <xsl:value-of select="@v151mpt_31" />
          </td>
          <td class="colW1">{lbl:v151mpt_32}</td>
          <td class="colW2">
            <xsl:value-of select="@v151mpt_32" />
          </td>
          <td class="colW1">{lbl:v151mpt_41}</td>
          <td class="colW2">
            <xsl:value-of select="@v151mpt_41" />
          </td>
        </tr>
        <tr>
          <td class="colW1">{lbl:v151mpt_42}</td>
          <td class="colW2">
            <xsl:value-of select="@v151mpt_42" />
          </td>
          <td class="colW1">{lbl:v151mpt_51}</td>
          <td class="colW2">
            <xsl:value-of select="@v151mpt_51" />
          </td>
          <td class="colW1">{lbl:v151mpt_52}</td>
          <td class="colW2">
            <xsl:value-of select="@v151mpt_52" />
          </td>
        </tr>
        <tr>
          <td class="colW1">{lbl:v152mpt_41}</td>
          <td class="colW2">
            <xsl:value-of select="@v152mpt_41" />
          </td>
          <td class="colW1">{lbl:v152mpt_42}</td>
          <td class="colW2">
            <xsl:value-of select="@v152mpt_42" />
          </td>
          <td class="colW1">{lbl:v152mpt_51}</td>
          <td class="colW2">
            <xsl:value-of select="@v152mpt_51" />
          </td>
        </tr>
        <tr>
          <td class="colW1">{lbl:v152mpt_52}</td>
          <td class="colW2">
            <xsl:value-of select="@v152mpt_52" />
          </td>
          <td class="colW1">{lbl:v153mpt_51}</td>
          <td class="colW2">
            <xsl:value-of select="@v153mpt_51" />
          </td>
          <td class="colW1">{lbl:v153mpt_52}</td>
          <td class="colW2">
            <xsl:value-of select="@v153mpt_52" />
          </td>
        </tr>
      </xsl:for-each>
      
      <xsl:for-each select="ag">
        <tr class="headerDetailNormal" >
          <td colspan="6">{lbl:ag}</td>
        </tr>
        <tr >
          <td class="colW1">{lbl:v151apt_31}</td>
          <td class="colW2">
            <xsl:value-of select="@v151apt_31" />
          </td>
          <td class="colW1">{lbl:v151apt_32}</td>
          <td class="colW2">
            <xsl:value-of select="@v151apt_32" />
          </td>
          <td class="colW1">{lbl:v151apt_41}</td>
          <td class="colW2">
            <xsl:value-of select="@v151apt_41" />
          </td>
        </tr>
        <tr>
          <td class="colW1">{lbl:v151apt_42}</td>
          <td class="colW2">
            <xsl:value-of select="@v151apt_42" />
          </td>
          <td class="colW1">{lbl:v151apt_51}</td>
          <td class="colW2">
            <xsl:value-of select="@v151apt_51" />
          </td>
          <td class="colW1">{lbl:v151apt_52}</td>
          <td class="colW2">
            <xsl:value-of select="@v151apt_52" />
          </td>
        </tr>
        <tr>
          <td class="colW1">{lbl:v152apt_41}</td>
          <td class="colW2">
            <xsl:value-of select="@v152apt_41" />
          </td>
          <td class="colW1">{lbl:v152apt_42}</td>
          <td class="colW2">
            <xsl:value-of select="@v152apt_42" />
          </td>
          <td class="colW1">{lbl:v152apt_51}</td>
          <td class="colW2">
            <xsl:value-of select="@v152apt_51" />
          </td>
        </tr>
        <tr>
          <td class="colW1">{lbl:v152apt_52}</td>
          <td class="colW2">
            <xsl:value-of select="@v152apt_52" />
          </td>
          <td class="colW1">{lbl:v153apt_51}</td>
          <td class="colW2">
            <xsl:value-of select="@v153apt_51" />
          </td>
          <td class="colW1">{lbl:v153apt_52}</td>
          <td class="colW2">
            <xsl:value-of select="@v153apt_52" />
          </td>
        </tr>
      </xsl:for-each>

      <xsl:choose>
        <xsl:when test="$IsInternal = 1">
          <xsl:for-each select="ibcpt">
            <tr class="headerDetailNormal" >
              <td colspan="6">{lbl:ibcpt}</td>
            </tr>
            <tr >
              <td class="colW1">{lbl:v151IBCpt_31}</td>
              <td class="colW2">
                <xsl:value-of select="@v151ibcpt_31" />
              </td>
              <td class="colW1">{lbl:v151IBCpt_41}</td>
              <td class="colW2">
                <xsl:value-of select="@v151ibcpt_41" />
              </td>
              <td class="colTitle">{lbl:v151IBCpt_51}</td>
              <td class="colValue">
                <xsl:value-of select="@v151ibcpt_51" />
              </td>
            </tr>
            <tr >
              <td class="colW1">{lbl:v152IBCpt_41}</td>
              <td class="colW2">
                <xsl:value-of select="@v152ibcpt_41" />
              </td>
              <td class="colW1">{lbl:v152IBCpt_51}</td>
              <td class="colW2">
                <xsl:value-of select="@v152ibcpt_51" />
              </td>
              <td class="colTitle">{lbl:v153IBCpt_51}</td>
              <td class="colValue">
                <xsl:value-of select="@v153ibcpt_51" />
              </td>
            </tr>
          </xsl:for-each>
        </xsl:when>
        <xsl:otherwise>
        </xsl:otherwise>
      </xsl:choose>
    </table>
  </xsl:template>
</xsl:stylesheet>
