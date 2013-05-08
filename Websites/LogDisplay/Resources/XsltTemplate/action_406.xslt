<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:param name="IsInternal" select="1"></xsl:param>
  <xsl:output method="html"/>
  <xsl:template match="acc_customerupdmemberhr_sag">
    <table class="tblDesc">
      <xsl:for-each select="info">
        <tr class="headerDetailNormal">
          <td colspan="6">{lbl:custInfo}</td>
        </tr>

        <tr >
          <td class="colW1">{lbl:SubAccName}</td>
          <td class="colW2">
            <xsl:value-of select="@subaccname" />
          </td>
          <td class="colW1">{lbl:IP}</td>
          <td class="colW2">
            <xsl:value-of select="@ip" />
          </td>
          <td class="colW1">{lbl:ErrCode}</td>
          <td class="colW2">
            <xsl:value-of select="@errcode" />
          </td>
        </tr>
        <tr >
          <td class="colW1">{lbl:ErrMsg}</td>
          <td class="colW2">
            <xsl:value-of select="@errmsg" />
          </td>
          <td class="colW1">{lbl:Language}</td>
          <td class="colW2">
            <xsl:value-of select="@language" />
          </td>
          <td class="colTitle"></td>
          <td class="colValue">

          </td>
        </tr>

      </xsl:for-each>
      <xsl:for-each select="s">
        <tr class="headerDetailNormal">
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
      <xsl:for-each select="au">
        <tr class="headerDetailNormal">
          <td colspan="6">{lbl:au}</td>
        </tr>
        <tr >
          <td class="colW1">{lbl:a_v151spt_31}</td>
          <td class="colW2">
            <xsl:value-of select="@a_v151spt_31" />
          </td>
          <td class="colW1">{lbl:a_v151spt_32}</td>
          <td class="colW2">
            <xsl:value-of select="@a_v151spt_32" />
          </td>
          <td class="colW1">{lbl:a_v151spt_41}</td>
          <td class="colW2">
            <xsl:value-of select="@a_v151spt_41" />
          </td>
        </tr>
        <tr>
          <td class="colW1">{lbl:a_v151spt_42}</td>
          <td class="colW2">
            <xsl:value-of select="@a_v151spt_42" />
          </td>
          <td class="colW1">{lbl:a_v151spt_51}</td>
          <td class="colW2">
            <xsl:value-of select="@a_v151spt_51" />
          </td>
          <td class="colW1">{lbl:a_v151spt_52}</td>
          <td class="colW2">
            <xsl:value-of select="@a_v151spt_52" />
          </td>
        </tr>
        <tr>
          <td class="colW1">{lbl:a_v152spt_41}</td>
          <td class="colW2">
            <xsl:value-of select="@a_v152spt_41" />
          </td>
          <td class="colW1">{lbl:a_v152spt_42}</td>
          <td class="colW2">
            <xsl:value-of select="@a_v152spt_42" />
          </td>
          <td class="colW1">{lbl:a_v152spt_51}</td>
          <td class="colW2">
            <xsl:value-of select="@a_v152spt_51" />
          </td>
        </tr>
        <tr>
          <td class="colW1">{lbl:a_v152spt_52}</td>
          <td class="colW2">
            <xsl:value-of select="@a_v152spt_52" />
          </td>
          <td class="colW1">{lbl:a_v153spt_51}</td>
          <td class="colW2">
            <xsl:value-of select="@a_v153spt_51" />
          </td>
          <td class="colW1">{lbl:a_v153spt_52}</td>
          <td class="colW2">
            <xsl:value-of select="@a_v153spt_52" />
          </td>
        </tr>
      </xsl:for-each>
      <xsl:for-each select="ia">
        <tr class="headerDetailNormal">
          <td colspan="6">{lbl:ia}</td>
        </tr>
        <tr >
          <td class="colW1">{lbl:isa_v151spt_31}</td>
          <td class="colW2">
            <xsl:value-of select="@isa_v151spt_31" />
          </td>
          <td class="colW1">{lbl:isa_v151spt_32}</td>
          <td class="colW2">
            <xsl:value-of select="@isa_v151spt_32" />
          </td>
          <td class="colW1">{lbl:isa_v151spt_41}</td>
          <td class="colW2">
            <xsl:value-of select="@isa_v151spt_41" />
          </td>
        </tr>
        <tr>
          <td class="colW1">{lbl:isa_v151spt_42}</td>
          <td class="colW2">
            <xsl:value-of select="@isa_v151spt_42" />
          </td>
          <td class="colW1">{lbl:isa_v151spt_51}</td>
          <td class="colW2">
            <xsl:value-of select="@isa_v151spt_51" />
          </td>
          <td class="colW1">{lbl:isa_v151spt_52}</td>
          <td class="colW2">
            <xsl:value-of select="@isa_v151spt_52" />
          </td>
        </tr>
        <tr>
          <td class="colW1">{lbl:isa_v152spt_41}</td>
          <td class="colW2">
            <xsl:value-of select="@isa_v152spt_41" />
          </td>
          <td class="colW1">{lbl:isa_v152spt_42}</td>
          <td class="colW2">
            <xsl:value-of select="@isa_v152spt_42" />
          </td>
          <td class="colW1">{lbl:isa_v152spt_51}</td>
          <td class="colW2">
            <xsl:value-of select="@isa_v152spt_51" />
          </td>
        </tr>
        <tr>
          <td class="colW1">{lbl:isa_v152spt_52}</td>
          <td class="colW2">
            <xsl:value-of select="@isa_v152spt_52" />
          </td>
          <td class="colW1">{lbl:isa_v153spt_51}</td>
          <td class="colW2">
            <xsl:value-of select="@isa_v153spt_51" />
          </td>
          <td class="colW1">{lbl:isa_v153spt_52}</td>
          <td class="colW2">
            <xsl:value-of select="@isa_v153spt_52" />
          </td>
        </tr>
      </xsl:for-each>
      <xsl:for-each select="isau">
        <tr class="headerDetailNormal">
          <td colspan="6">{lbl:isau}</td>
        </tr>
        <tr >
          <td class="colW1">{lbl:isa_v151smpt_31}</td>
          <td class="colW2">
            <xsl:value-of select="@isa_v151smpt_31" />
          </td>
          <td class="colW1">{lbl:isa_v151smpt_32}</td>
          <td class="colW2">
            <xsl:value-of select="@isa_v151smpt_32" />
          </td>
          <td class="colW1">{lbl:isa_v151smpt_41}</td>
          <td class="colW2">
            <xsl:value-of select="@isa_v151smpt_41" />
          </td>
        </tr>
        <tr >
          <td class="colW1">{lbl:isa_v151smpt_42}</td>
          <td class="colW2">
            <xsl:value-of select="@isa_v151smpt_42" />
          </td>
          <td class="colW1">{lbl:isa_v151smpt_51}</td>
          <td class="colW2">
            <xsl:value-of select="@isa_v151smpt_51" />
          </td>
          <td class="colW1">{lbl:isa_v151smpt_52}</td>
          <td class="colW2">
            <xsl:value-of select="@isa_v151smpt_52" />
          </td>
        </tr>
        <tr >
          <td class="colW1">{lbl:isa_v152smpt_41}</td>
          <td class="colW2">
            <xsl:value-of select="@isa_v152smpt_41" />
          </td>
          <td class="colW1">{lbl:isa_v152smpt_42}</td>
          <td class="colW2">
            <xsl:value-of select="@isa_v152smpt_42" />
          </td>
          <td class="colW1">{lbl:isa_v152smpt_51}</td>
          <td class="colW2">
            <xsl:value-of select="@isa_v152smpt_51" />
          </td>
        </tr>
        <tr >
          <td class="colW1">{lbl:isa_v152smpt_52}</td>
          <td class="colW2">
            <xsl:value-of select="@isa_v152smpt_52" />
          </td>
          <td class="colW1">{lbl:isa_v153smpt_51}</td>
          <td class="colW2">
            <xsl:value-of select="@isa_v153smpt_51" />
          </td>
          <td class="colW1">{lbl:isa_v153smpt_52}</td>
          <td class="colW2">
            <xsl:value-of select="@isa_v153smpt_52" />
          </td>
        </tr>
      </xsl:for-each>
    </table>
  </xsl:template>


</xsl:stylesheet>
